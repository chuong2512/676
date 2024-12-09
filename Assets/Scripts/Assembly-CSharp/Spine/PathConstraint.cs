using System;

namespace Spine
{
	public class PathConstraint : IConstraint, IUpdatable
	{
		private const int NONE = -1;

		private const int BEFORE = -2;

		private const int AFTER = -3;

		private const float Epsilon = 1E-05f;

		internal PathConstraintData data;

		internal ExposedList<Bone> bones;

		internal Slot target;

		internal float position;

		internal float spacing;

		internal float rotateMix;

		internal float translateMix;

		internal ExposedList<float> spaces = new ExposedList<float>();

		internal ExposedList<float> positions = new ExposedList<float>();

		internal ExposedList<float> world = new ExposedList<float>();

		internal ExposedList<float> curves = new ExposedList<float>();

		internal ExposedList<float> lengths = new ExposedList<float>();

		internal float[] segments = new float[10];

		public int Order => data.order;

		public float Position
		{
			get
			{
				return position;
			}
			set
			{
				position = value;
			}
		}

		public float Spacing
		{
			get
			{
				return spacing;
			}
			set
			{
				spacing = value;
			}
		}

		public float RotateMix
		{
			get
			{
				return rotateMix;
			}
			set
			{
				rotateMix = value;
			}
		}

		public float TranslateMix
		{
			get
			{
				return translateMix;
			}
			set
			{
				translateMix = value;
			}
		}

		public ExposedList<Bone> Bones => bones;

		public Slot Target
		{
			get
			{
				return target;
			}
			set
			{
				target = value;
			}
		}

		public PathConstraintData Data => data;

		public PathConstraint(PathConstraintData data, Skeleton skeleton)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data", "data cannot be null.");
			}
			if (skeleton == null)
			{
				throw new ArgumentNullException("skeleton", "skeleton cannot be null.");
			}
			this.data = data;
			bones = new ExposedList<Bone>(data.Bones.Count);
			foreach (BoneData bone in data.bones)
			{
				bones.Add(skeleton.FindBone(bone.name));
			}
			target = skeleton.FindSlot(data.target.name);
			position = data.position;
			spacing = data.spacing;
			rotateMix = data.rotateMix;
			translateMix = data.translateMix;
		}

		public PathConstraint(PathConstraint constraint, Skeleton skeleton)
		{
			if (constraint == null)
			{
				throw new ArgumentNullException("constraint cannot be null.");
			}
			if (skeleton == null)
			{
				throw new ArgumentNullException("skeleton cannot be null.");
			}
			data = constraint.data;
			bones = new ExposedList<Bone>(constraint.Bones.Count);
			foreach (Bone bone in constraint.Bones)
			{
				bones.Add(skeleton.Bones.Items[bone.data.index]);
			}
			target = skeleton.slots.Items[constraint.target.data.index];
			position = constraint.position;
			spacing = constraint.spacing;
			rotateMix = constraint.rotateMix;
			translateMix = constraint.translateMix;
		}

		public void Apply()
		{
			Update();
		}

		public void Update()
		{
			PathAttachment pathAttachment = target.Attachment as PathAttachment;
			if (pathAttachment == null)
			{
				return;
			}
			float num = rotateMix;
			float num2 = translateMix;
			bool num3 = num2 > 0f;
			bool flag = num > 0f;
			if (!num3 && !flag)
			{
				return;
			}
			PathConstraintData pathConstraintData = data;
			bool flag2 = pathConstraintData.spacingMode == SpacingMode.Percent;
			RotateMode rotateMode = pathConstraintData.rotateMode;
			bool flag3 = rotateMode == RotateMode.Tangent;
			bool flag4 = rotateMode == RotateMode.ChainScale;
			int count = bones.Count;
			int num4 = (flag3 ? count : (count + 1));
			Bone[] items = bones.Items;
			ExposedList<float> exposedList = spaces.Resize(num4);
			ExposedList<float> exposedList2 = null;
			float num5 = spacing;
			if (flag4 || !flag2)
			{
				if (flag4)
				{
					exposedList2 = lengths.Resize(count);
				}
				bool flag5 = pathConstraintData.spacingMode == SpacingMode.Length;
				int num6 = 0;
				int num7 = num4 - 1;
				while (num6 < num7)
				{
					Bone bone = items[num6];
					float length = bone.data.length;
					if (length < 1E-05f)
					{
						if (flag4)
						{
							exposedList2.Items[num6] = 0f;
						}
						exposedList.Items[++num6] = 0f;
					}
					else if (flag2)
					{
						if (flag4)
						{
							float num8 = length * bone.a;
							float num9 = length * bone.c;
							float num10 = (float)Math.Sqrt(num8 * num8 + num9 * num9);
							exposedList2.Items[num6] = num10;
						}
						exposedList.Items[++num6] = num5;
					}
					else
					{
						float num11 = length * bone.a;
						float num12 = length * bone.c;
						float num13 = (float)Math.Sqrt(num11 * num11 + num12 * num12);
						if (flag4)
						{
							exposedList2.Items[num6] = num13;
						}
						exposedList.Items[++num6] = (flag5 ? (length + num5) : num5) * num13 / length;
					}
				}
			}
			else
			{
				for (int i = 1; i < num4; i++)
				{
					exposedList.Items[i] = num5;
				}
			}
			float[] array = ComputeWorldPositions(pathAttachment, num4, flag3, pathConstraintData.positionMode == PositionMode.Percent, flag2);
			float num14 = array[0];
			float num15 = array[1];
			float num16 = pathConstraintData.offsetRotation;
			bool flag6;
			if (num16 == 0f)
			{
				flag6 = rotateMode == RotateMode.Chain;
			}
			else
			{
				flag6 = false;
				Bone bone2 = target.bone;
				num16 *= ((bone2.a * bone2.d - bone2.b * bone2.c > 0f) ? ((float)Math.PI / 180f) : (-(float)Math.PI / 180f));
			}
			int num17 = 0;
			int num18 = 3;
			while (num17 < count)
			{
				Bone bone3 = items[num17];
				bone3.worldX += (num14 - bone3.worldX) * num2;
				bone3.worldY += (num15 - bone3.worldY) * num2;
				float num19 = array[num18];
				float num20 = array[num18 + 1];
				float num21 = num19 - num14;
				float num22 = num20 - num15;
				if (flag4)
				{
					float num23 = exposedList2.Items[num17];
					if (num23 >= 1E-05f)
					{
						float num24 = ((float)Math.Sqrt(num21 * num21 + num22 * num22) / num23 - 1f) * num + 1f;
						bone3.a *= num24;
						bone3.c *= num24;
					}
				}
				num14 = num19;
				num15 = num20;
				if (flag)
				{
					float a = bone3.a;
					float b = bone3.b;
					float c = bone3.c;
					float d = bone3.d;
					float num25 = (flag3 ? array[num18 - 1] : ((!(exposedList.Items[num17 + 1] < 1E-05f)) ? MathUtils.Atan2(num22, num21) : array[num18 + 2]));
					num25 -= MathUtils.Atan2(c, a);
					float num26;
					float num27;
					if (flag6)
					{
						num26 = MathUtils.Cos(num25);
						num27 = MathUtils.Sin(num25);
						float length2 = bone3.data.length;
						num14 += (length2 * (num26 * a - num27 * c) - num21) * num;
						num15 += (length2 * (num27 * a + num26 * c) - num22) * num;
					}
					else
					{
						num25 += num16;
					}
					if (num25 > (float)Math.PI)
					{
						num25 -= (float)Math.PI * 2f;
					}
					else if (num25 < -(float)Math.PI)
					{
						num25 += (float)Math.PI * 2f;
					}
					num25 *= num;
					num26 = MathUtils.Cos(num25);
					num27 = MathUtils.Sin(num25);
					bone3.a = num26 * a - num27 * c;
					bone3.b = num26 * b - num27 * d;
					bone3.c = num27 * a + num26 * c;
					bone3.d = num27 * b + num26 * d;
				}
				bone3.appliedValid = false;
				num17++;
				num18 += 3;
			}
		}

		private float[] ComputeWorldPositions(PathAttachment path, int spacesCount, bool tangents, bool percentPosition, bool percentSpacing)
		{
			Slot slot = target;
			float num = position;
			float[] items = spaces.Items;
			float[] items2 = positions.Resize(spacesCount * 3 + 2).Items;
			bool closed = path.Closed;
			int worldVerticesLength = path.WorldVerticesLength;
			int num2 = worldVerticesLength / 6;
			int num3 = -1;
			float num4 = 0f;
			float[] items3;
			if (!path.ConstantSpeed)
			{
				float[] array = path.Lengths;
				num2 -= (closed ? 1 : 2);
				num4 = array[num2];
				if (percentPosition)
				{
					num *= num4;
				}
				if (percentSpacing)
				{
					for (int i = 1; i < spacesCount; i++)
					{
						items[i] *= num4;
					}
				}
				items3 = world.Resize(8).Items;
				int j = 0;
				int k = 0;
				int num5 = 0;
				for (; j < spacesCount; j++, k += 3)
				{
					float num6 = items[j];
					num += num6;
					float num7 = num;
					if (closed)
					{
						num7 %= num4;
						if (num7 < 0f)
						{
							num7 += num4;
						}
						num5 = 0;
					}
					else
					{
						if (num7 < 0f)
						{
							if (num3 != -2)
							{
								num3 = -2;
								path.ComputeWorldVertices(slot, 2, 4, items3, 0);
							}
							AddBeforePosition(num7, items3, 0, items2, k);
							continue;
						}
						if (num7 > num4)
						{
							if (num3 != -3)
							{
								num3 = -3;
								path.ComputeWorldVertices(slot, worldVerticesLength - 6, 4, items3, 0);
							}
							AddAfterPosition(num7 - num4, items3, 0, items2, k);
							continue;
						}
					}
					float num8;
					while (true)
					{
						num8 = array[num5];
						if (!(num7 > num8))
						{
							break;
						}
						num5++;
					}
					if (num5 == 0)
					{
						num7 /= num8;
					}
					else
					{
						float num9 = array[num5 - 1];
						num7 = (num7 - num9) / (num8 - num9);
					}
					if (num5 != num3)
					{
						num3 = num5;
						if (closed && num5 == num2)
						{
							path.ComputeWorldVertices(slot, worldVerticesLength - 4, 4, items3, 0);
							path.ComputeWorldVertices(slot, 0, 4, items3, 4);
						}
						else
						{
							path.ComputeWorldVertices(slot, num5 * 6 + 2, 8, items3, 0);
						}
					}
					AddCurvePosition(num7, items3[0], items3[1], items3[2], items3[3], items3[4], items3[5], items3[6], items3[7], items2, k, tangents || (j > 0 && num6 < 1E-05f));
				}
				return items2;
			}
			if (closed)
			{
				worldVerticesLength += 2;
				items3 = world.Resize(worldVerticesLength).Items;
				path.ComputeWorldVertices(slot, 2, worldVerticesLength - 4, items3, 0);
				path.ComputeWorldVertices(slot, 0, 2, items3, worldVerticesLength - 4);
				items3[worldVerticesLength - 2] = items3[0];
				items3[worldVerticesLength - 1] = items3[1];
			}
			else
			{
				num2--;
				worldVerticesLength -= 4;
				items3 = world.Resize(worldVerticesLength).Items;
				path.ComputeWorldVertices(slot, 2, worldVerticesLength, items3, 0);
			}
			float[] items4 = curves.Resize(num2).Items;
			num4 = 0f;
			float num10 = items3[0];
			float num11 = items3[1];
			float num12 = 0f;
			float num13 = 0f;
			float num14 = 0f;
			float num15 = 0f;
			float num16 = 0f;
			float num17 = 0f;
			int num18 = 0;
			int num19 = 2;
			while (num18 < num2)
			{
				num12 = items3[num19];
				num13 = items3[num19 + 1];
				num14 = items3[num19 + 2];
				num15 = items3[num19 + 3];
				num16 = items3[num19 + 4];
				num17 = items3[num19 + 5];
				float num20 = (num10 - num12 * 2f + num14) * 0.1875f;
				float num21 = (num11 - num13 * 2f + num15) * 0.1875f;
				float num22 = ((num12 - num14) * 3f - num10 + num16) * (3f / 32f);
				float num23 = ((num13 - num15) * 3f - num11 + num17) * (3f / 32f);
				float num24 = num20 * 2f + num22;
				float num25 = num21 * 2f + num23;
				float num26 = (num12 - num10) * 0.75f + num20 + num22 * (355f / (678f * (float)Math.PI));
				float num27 = (num13 - num11) * 0.75f + num21 + num23 * (355f / (678f * (float)Math.PI));
				num4 += (float)Math.Sqrt(num26 * num26 + num27 * num27);
				num26 += num24;
				num27 += num25;
				num24 += num22;
				num25 += num23;
				num4 += (float)Math.Sqrt(num26 * num26 + num27 * num27);
				num26 += num24;
				num27 += num25;
				num4 += (float)Math.Sqrt(num26 * num26 + num27 * num27);
				num26 += num24 + num22;
				num27 += num25 + num23;
				num4 = (items4[num18] = num4 + (float)Math.Sqrt(num26 * num26 + num27 * num27));
				num10 = num16;
				num11 = num17;
				num18++;
				num19 += 6;
			}
			num = ((!percentPosition) ? (num * (num4 / path.lengths[num2 - 1])) : (num * num4));
			if (percentSpacing)
			{
				for (int l = 1; l < spacesCount; l++)
				{
					items[l] *= num4;
				}
			}
			float[] array2 = segments;
			float num28 = 0f;
			int m = 0;
			int n = 0;
			int num29 = 0;
			int num30 = 0;
			for (; m < spacesCount; m++, n += 3)
			{
				float num31 = items[m];
				num += num31;
				float num32 = num;
				if (closed)
				{
					num32 %= num4;
					if (num32 < 0f)
					{
						num32 += num4;
					}
					num29 = 0;
				}
				else
				{
					if (num32 < 0f)
					{
						AddBeforePosition(num32, items3, 0, items2, n);
						continue;
					}
					if (num32 > num4)
					{
						AddAfterPosition(num32 - num4, items3, worldVerticesLength - 4, items2, n);
						continue;
					}
				}
				float num33;
				while (true)
				{
					num33 = items4[num29];
					if (!(num32 > num33))
					{
						break;
					}
					num29++;
				}
				if (num29 == 0)
				{
					num32 /= num33;
				}
				else
				{
					float num34 = items4[num29 - 1];
					num32 = (num32 - num34) / (num33 - num34);
				}
				if (num29 != num3)
				{
					num3 = num29;
					int num35 = num29 * 6;
					num10 = items3[num35];
					num11 = items3[num35 + 1];
					num12 = items3[num35 + 2];
					num13 = items3[num35 + 3];
					num14 = items3[num35 + 4];
					num15 = items3[num35 + 5];
					num16 = items3[num35 + 6];
					num17 = items3[num35 + 7];
					float num20 = (num10 - num12 * 2f + num14) * 0.03f;
					float num21 = (num11 - num13 * 2f + num15) * 0.03f;
					float num22 = ((num12 - num14) * 3f - num10 + num16) * 0.006f;
					float num23 = ((num13 - num15) * 3f - num11 + num17) * 0.006f;
					float num24 = num20 * 2f + num22;
					float num25 = num21 * 2f + num23;
					float num26 = (num12 - num10) * 0.3f + num20 + num22 * (355f / (678f * (float)Math.PI));
					float num27 = (num13 - num11) * 0.3f + num21 + num23 * (355f / (678f * (float)Math.PI));
					num28 = (array2[0] = (float)Math.Sqrt(num26 * num26 + num27 * num27));
					for (num35 = 1; num35 < 8; num35++)
					{
						num26 += num24;
						num27 += num25;
						num24 += num22;
						num25 += num23;
						num28 = (array2[num35] = num28 + (float)Math.Sqrt(num26 * num26 + num27 * num27));
					}
					num26 += num24;
					num27 += num25;
					num28 = (array2[8] = num28 + (float)Math.Sqrt(num26 * num26 + num27 * num27));
					num26 += num24 + num22;
					num27 += num25 + num23;
					num28 = (array2[9] = num28 + (float)Math.Sqrt(num26 * num26 + num27 * num27));
					num30 = 0;
				}
				num32 *= num28;
				float num36;
				while (true)
				{
					num36 = array2[num30];
					if (!(num32 > num36))
					{
						break;
					}
					num30++;
				}
				if (num30 == 0)
				{
					num32 /= num36;
				}
				else
				{
					float num37 = array2[num30 - 1];
					num32 = (float)num30 + (num32 - num37) / (num36 - num37);
				}
				AddCurvePosition(num32 * 0.1f, num10, num11, num12, num13, num14, num15, num16, num17, items2, n, tangents || (m > 0 && num31 < 1E-05f));
			}
			return items2;
		}

		private static void AddBeforePosition(float p, float[] temp, int i, float[] output, int o)
		{
			float num = temp[i];
			float num2 = temp[i + 1];
			float x = temp[i + 2] - num;
			float num3 = MathUtils.Atan2(temp[i + 3] - num2, x);
			output[o] = num + p * MathUtils.Cos(num3);
			output[o + 1] = num2 + p * MathUtils.Sin(num3);
			output[o + 2] = num3;
		}

		private static void AddAfterPosition(float p, float[] temp, int i, float[] output, int o)
		{
			float num = temp[i + 2];
			float num2 = temp[i + 3];
			float x = num - temp[i];
			float num3 = MathUtils.Atan2(num2 - temp[i + 1], x);
			output[o] = num + p * MathUtils.Cos(num3);
			output[o + 1] = num2 + p * MathUtils.Sin(num3);
			output[o + 2] = num3;
		}

		private static void AddCurvePosition(float p, float x1, float y1, float cx1, float cy1, float cx2, float cy2, float x2, float y2, float[] output, int o, bool tangents)
		{
			if (p < 1E-05f || float.IsNaN(p))
			{
				output[o] = x1;
				output[o + 1] = y1;
				output[o + 2] = (float)Math.Atan2(cy1 - y1, cx1 - x1);
				return;
			}
			float num = p * p;
			float num2 = num * p;
			float num3 = 1f - p;
			float num4 = num3 * num3;
			float num5 = num4 * num3;
			float num6 = num3 * p;
			float num7 = num6 * 3f;
			float num8 = num3 * num7;
			float num9 = num7 * p;
			float num10 = x1 * num5 + cx1 * num8 + cx2 * num9 + x2 * num2;
			float num11 = y1 * num5 + cy1 * num8 + cy2 * num9 + y2 * num2;
			output[o] = num10;
			output[o + 1] = num11;
			if (tangents)
			{
				if (p < 0.001f)
				{
					output[o + 2] = (float)Math.Atan2(cy1 - y1, cx1 - x1);
				}
				else
				{
					output[o + 2] = (float)Math.Atan2(num11 - (y1 * num4 + cy1 * num6 * 2f + cy2 * num), num10 - (x1 * num4 + cx1 * num6 * 2f + cx2 * num));
				}
			}
		}

		public override string ToString()
		{
			return data.name;
		}
	}
}

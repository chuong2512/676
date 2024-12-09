using System;

namespace Spine
{
	public class Slot
	{
		internal SlotData data;

		internal Bone bone;

		internal float r;

		internal float g;

		internal float b;

		internal float a;

		internal float r2;

		internal float g2;

		internal float b2;

		internal bool hasSecondColor;

		internal Attachment attachment;

		internal float attachmentTime;

		internal ExposedList<float> attachmentVertices = new ExposedList<float>();

		public SlotData Data => data;

		public Bone Bone => bone;

		public Skeleton Skeleton => bone.skeleton;

		public float R
		{
			get
			{
				return r;
			}
			set
			{
				r = value;
			}
		}

		public float G
		{
			get
			{
				return g;
			}
			set
			{
				g = value;
			}
		}

		public float B
		{
			get
			{
				return b;
			}
			set
			{
				b = value;
			}
		}

		public float A
		{
			get
			{
				return a;
			}
			set
			{
				a = value;
			}
		}

		public float R2
		{
			get
			{
				return r2;
			}
			set
			{
				r2 = value;
			}
		}

		public float G2
		{
			get
			{
				return g2;
			}
			set
			{
				g2 = value;
			}
		}

		public float B2
		{
			get
			{
				return b2;
			}
			set
			{
				b2 = value;
			}
		}

		public bool HasSecondColor
		{
			get
			{
				return data.hasSecondColor;
			}
			set
			{
				data.hasSecondColor = value;
			}
		}

		public Attachment Attachment
		{
			get
			{
				return attachment;
			}
			set
			{
				if (attachment != value)
				{
					attachment = value;
					attachmentTime = bone.skeleton.time;
					attachmentVertices.Clear(clearArray: false);
				}
			}
		}

		public float AttachmentTime
		{
			get
			{
				return bone.skeleton.time - attachmentTime;
			}
			set
			{
				attachmentTime = bone.skeleton.time - value;
			}
		}

		public ExposedList<float> AttachmentVertices
		{
			get
			{
				return attachmentVertices;
			}
			set
			{
				if (attachmentVertices == null)
				{
					throw new ArgumentNullException("attachmentVertices", "attachmentVertices cannot be null.");
				}
				attachmentVertices = value;
			}
		}

		public Slot(SlotData data, Bone bone)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data", "data cannot be null.");
			}
			if (bone == null)
			{
				throw new ArgumentNullException("bone", "bone cannot be null.");
			}
			this.data = data;
			this.bone = bone;
			if (data.hasSecondColor)
			{
				r2 = (g2 = (b2 = 0f));
			}
			SetToSetupPose();
		}

		public Slot(Slot slot, Bone bone)
		{
			if (slot == null)
			{
				throw new ArgumentNullException("slot", "slot cannot be null.");
			}
			if (bone == null)
			{
				throw new ArgumentNullException("bone", "bone cannot be null.");
			}
			data = slot.data;
			this.bone = bone;
			r = slot.r;
			g = slot.g;
			b = slot.b;
			a = slot.a;
			if (slot.hasSecondColor)
			{
				r2 = slot.r2;
				g2 = slot.g2;
				b2 = slot.b2;
			}
			else
			{
				r2 = (g2 = (b2 = 0f));
			}
			hasSecondColor = slot.hasSecondColor;
			attachment = slot.attachment;
			attachmentTime = slot.attachmentTime;
		}

		public void SetToSetupPose()
		{
			r = data.r;
			g = data.g;
			b = data.b;
			a = data.a;
			if (HasSecondColor)
			{
				r2 = data.r2;
				g2 = data.g2;
				b2 = data.b2;
			}
			if (data.attachmentName == null)
			{
				Attachment = null;
				return;
			}
			attachment = null;
			Attachment = bone.skeleton.GetAttachment(data.index, data.attachmentName);
		}

		public override string ToString()
		{
			return data.name;
		}
	}
}

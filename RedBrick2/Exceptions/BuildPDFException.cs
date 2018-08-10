using System;
using System.Runtime.Serialization;

namespace Exceptions {
	[Serializable]
	internal class BuildPDFException : Exception {
		public BuildPDFException() {
		}

		public BuildPDFException(string message) : base(message) {
		}

		public BuildPDFException(string message, Exception innerException) : base(message, innerException) {
		}

		protected BuildPDFException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoForm
{
	public interface IInputContainer
	{
		public IInput<T> GetInputFor<T>();
	}
	public sealed class InputContainer : IInputContainer
	{
		public IInput<T> GetInputFor<T>()
		{
			throw new NotImplementedException();
		}
	}
}

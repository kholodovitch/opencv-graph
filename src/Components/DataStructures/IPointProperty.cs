using System.Drawing;

namespace DataStructures
{
	public interface IPointProperty : IFilterProperty
	{
		Point Point { get; set; }
	}
}
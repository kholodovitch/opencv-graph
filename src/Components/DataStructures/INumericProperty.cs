namespace DataStructures
{
	public interface INumericProperty : IFilterProperty
	{
		decimal Max { get; }

		decimal Min { get; }

		decimal Step { get; }

		int DecimalPlaces { get; }
	}
}
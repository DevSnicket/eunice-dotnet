using System.Collections;

class Class {
	#pragma warning disable CS0414
	static FieldClass _field;
	#pragma warning restore CS0414

	static IEnumerable Method() {
		_field = null;

		yield return null;
	}
}
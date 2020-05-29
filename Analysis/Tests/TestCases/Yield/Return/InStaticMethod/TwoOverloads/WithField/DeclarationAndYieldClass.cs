using System;
using System.Collections;

class DeclarationAndYieldClass {
	#pragma warning disable CS0414
	static FieldForParameterlessClass _fieldForParameterless;
	#pragma warning restore CS0414

	static IEnumerable Method() {
		_fieldForParameterless = null;

		yield return null;
	}

	#pragma warning disable CS0414
	static FieldForParameterizedClass _fieldForParameterized;
	#pragma warning restore CS0414

	static IEnumerable Method(
		Object parameter
	) {
		_fieldForParameterized = null;

		yield return null;
	}
}
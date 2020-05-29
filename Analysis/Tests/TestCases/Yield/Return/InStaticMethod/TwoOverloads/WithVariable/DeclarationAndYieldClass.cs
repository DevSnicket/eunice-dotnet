using System;
using System.Collections;

class DeclarationAndYieldClass {
	static IEnumerable Method() {
		#pragma warning disable CS0168
		VariableForParameterlessClass variable;
		#pragma warning restore CS0168

		yield return null;
	}

	static IEnumerable Method(
		Object parameter
	) {
		#pragma warning disable CS0168
		VariableForParameterizedClass variable;
		#pragma warning restore CS0168

		yield return null;
	}
}
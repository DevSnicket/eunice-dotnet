using System.Collections;

class DeclarationAndYieldClass {
	static IEnumerable Method() {
		#pragma warning disable CS0168
		VariableClass variable;
		#pragma warning restore CS0168

		yield return null;
	}
}
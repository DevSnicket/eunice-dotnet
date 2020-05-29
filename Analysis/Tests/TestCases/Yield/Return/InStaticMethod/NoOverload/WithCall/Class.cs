using System.Collections;

class Class {
	static IEnumerable Caller() {
		Callee();

		yield return null;
	}

	static void Callee() { }
}
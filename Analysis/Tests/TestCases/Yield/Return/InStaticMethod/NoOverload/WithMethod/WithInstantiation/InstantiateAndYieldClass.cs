using System.Collections;

class InstantiateAndYieldClass {
	static void InstantiateMethod() {
		new InstantiatedClass();
	}

	static IEnumerable YieldMethod() {
		yield return null;
	}
}
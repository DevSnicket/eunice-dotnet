using System.Collections;

abstract class Class {
	public abstract void AbstractMethod();

	static IEnumerable YieldMethod() {
		yield return null;
	}
}
using System.Collections.Generic;

class YieldClass {
	static IEnumerable<ReturnClass> Method() {
		yield return null;
	}
}
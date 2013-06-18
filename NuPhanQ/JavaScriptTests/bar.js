/// <reference path="Scripts/jquery-1.4.4-vsdoc.js" />
/// <reference path="Scripts/qunit.js" />

$(function () {
    test("Bar test 1", function () {
        ok(true, "Should be ok");
	});
    
	test("Bar test 2", function () {
	    equal("x", "x", "Should be ok");
	});
});

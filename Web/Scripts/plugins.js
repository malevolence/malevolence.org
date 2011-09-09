//$.ajaxSetup({
//	type: 'POST',
//	dataType: 'json',
//	contentType: 'application/json; charset=utf-8'
//});

//$('#log').ajaxError(function(e, xhr, settings, exc) {
//	$(this).text('Ajax Error: ' + xhr.statusText);
//});

window.log = function() {
	log.history = log.history || [];
	log.history.push(arguments);
	if (this.console) {
		console.log(Array.prototype.slice.call(arguments));
	}
};
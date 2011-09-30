var totalFileSize = 0;

$(function () {
	$('#droparea').bind('dragenter', function (e) {
		e.preventDefault();
		e.stopPropagation();
	}).bind('dragover', function (e) {
		e.preventDefault();
		e.stopPropagation();
		$(this).addClass('over');
	}).bind('dragleave', function (e) {
		e.preventDefault();
		e.stopPropagation();
		$(this).removeClass('over');
	})[0].addEventListener('drop', onDrop, false);

	$('<div>', {
		id: 'file-list'
	}).insertAfter('#totalFileSize');
});

function onDrop(e) {
	e.preventDefault();
	e.stopPropagation();
	$(this).removeClass('over');

	var readFileSize = 0;
	var files = e.dataTransfer.files;
	var currentProgress;

	for (var i = 0, file; file = files[i]; i++) {
		readFileSize += file.fileSize;

		// only process image files
		var imageType = /image.*/;

		if (!file.type.match(imageType)) {
			continue;
		}

		var reader = new FileReader();

		reader.onerror = function (e) {
			console.log(e.target.error);
		}

		reader.onload = (function (theFile) {
			return function (e) {
				var deg = Math.floor(Math.random() * 31);
				deg = Math.floor(Math.random() * 2) ? deg : -deg;

				var data = {
					'file': {
						'name': theFile.name,
						'src': e.target.result,
						'size': Math.round(theFile.fileSize / 1024),
						'type': theFile.type
					}
				};

				currentProgress = $('#tmplProgress').tmpl(data).appendTo('#file-list');
				uploadFile(theFile, currentProgress);
			};
		})(file);

		reader.readAsDataURL(file);
	}

	totalFileSize += readFileSize;
	var sizeLabel = ['Just read: ', Math.round(readFileSize / 1024), ' KB, ', 'Total gallery size: ', Math.round(totalFileSize / 1024), ' KB'].join('');

	$('#totalFileSize').text(sizeLabel);
}

function uploadFile(file, progress) {
	var xhr = new XMLHttpRequest();

	xhr.upload.addEventListener('progress', function (e) {
		if (e.lengthComputable) {
			var $progressBar = $(progress).find('.progress-bar');
			$progressBar.text('Uploading ' + file.name + '. Size: ' + Math.round(file.fileSize / 1024) + ' KB. ' + (e.loaded / e.total) * 100 + '% done.');
			$progressBar.css('width', (e.loaded / e.total) * 100 + '%');
		}
	}, false);

	xhr.addEventListener('load', function () {
		$(progress).addClass('uploaded');
		$(progress).find('.progress-bar').text('Upload Complete: ' + file.name + '. Size: ' + Math.round(file.fileSize / 1024) + ' KB. Type: ' + file.type);
	}, false);

	var url = '/Admin/Gallery/AddPhotoAjax';
	xhr.open('POST', url, true);

	var formData = new FormData();
	formData.append('id', $('#albumID').val());
	formData.append('file', file);

	// Send the file
	xhr.send(formData);
}
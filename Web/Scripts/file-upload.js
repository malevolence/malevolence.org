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
	}).appendTo('.main-content');
});

function onDrop(e) {
	e.preventDefault();
	e.stopPropagation();
	$(this).removeClass('over');

	console.log(e);

	var readFileSize = 0;
	var files = e.dataTransfer.files;

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

//		reader.onload = (function (theFile) {
//			return function (e) {
//				var deg = Math.floor(Math.random() * 31);
//				deg = Math.floor(Math.random() * 2) ? deg : -deg;

//				var data = {
//					'file': {
//						'name': theFile.name,
//						'src': e.target.result,
//						'fileSize': theFile.fileSize,
//						'type': theFile.type,
//						'rotate': deg
//					}
//				};

//				$('#tmplThumbnail').tmpl(data).appendTo('#thumbnails');
//			};
//		})(file);

		reader.readAsDataURL(file);
		uploadFile(file);
	}

	totalFileSize += readFileSize;
	var sizeLabel = ['Just read: ', Math.round(readFileSize / 1024), ' KB, ', 'Total gallery size: ', Math.round(totalFileSize / 1024), ' KB'].join('');

	$('#totalFileSize').text(sizeLabel);
}

function uploadFile(file) {
	var li = $('<li>');
	var container = $('<div>', {
		'class': 'progress-bar-container'
	});
	
	var progressBar = $('<div>', {
		'class': 'progress-bar'
	});

	container.append(progressBar);
	li.append(container);

	var fileName = file.fileName;
	var fileSize = file.fileSize;
	var fileType = file.type;

	var xhr = new XMLHttpRequest();

	xhr.upload.addEventListener('progress', function (e) {
		if (e.lengthComputable) {
			progressBar.text('Uploading ' + fileName + '. Size: ' + Math.round(fileSize / 1024) + ' KB. Type: ' + fileType);
			progressBar.css('width', (e.loaded / e.total) * 100 + '%');
		}
	}, false);

	xhr.addEventListener('load', function () {
		progressBar.addClass('uploaded');
		progressBar.text('Upload Complete: ' + fileName + '. Size: ' + Math.round(fileSize / 1024) + ' KB. Type: ' + fileType);
	}, false);

	var url = '/Admin/Gallery/AddPhotoDragDropAsync';
	xhr.open('POST', url, true);

	// Set some headers
	xhr.setRequestHeader('Content-Type', 'multipart/form-data');
	xhr.setRequestHeader('X-File-Name', fileName);
	xhr.setRequestHeader('X-File-Size', fileSize);
	xhr.setRequestHeader('X-File-Type', fileType);
	xhr.setRequestHeader('AlbumID', $('#albumID').val());

	// Send the file
	xhr.send(file);

	$('#file-list').prepend(li);
}

/*
 * jQuery File Upload Plugin JS Example 8.9.1
 * https://github.com/blueimp/jQuery-File-Upload
 *
 * Copyright 2010, Sebastian Tschan
 * https://blueimp.net
 *
 * Licensed under the MIT license:
 * http://www.opensource.org/licenses/MIT
 */

/* global $, window */

var Desc;
var documentid;
/*jslint unparam: true */
/*global window, $ */
$(document).ready(function () {
    'use strict';
      
    $('#fileupload').fileupload({
        type:'GET',  
        url: '/SignalRNotify/ProfilePhotoUpload',
        dataType: 'json',
        done: function (e, data) {
            if (data.result.flag == "true")
            {                               
                $("#Profilepic").attr('src', data.result.path);
            }
            else
            {

            }
            //$.each(data.result.files, function (index, file) {
              
            //});
        },
        progressall: function (e, data) {                     
            var progress = parseInt(data.loaded / data.total * 100, 10);            
            $('#Profilepic').attr('src', '../Content/img/loader1.gif');
            //$('.progress-bar').css(
            //    'width',
            //    progress + '%'
            //);

            
        },
        add: function (e, data) {
           
            data.submit();
        }
    }).prop('disabled', !$.support.fileInput)
        .parent().addClass($.support.fileInput ? undefined : 'disabled');

    $('#fileupload').bind('fileuploadstop', function (e) {
      
    });

    $('#fileupload').bind('fileuploadadd', function (e, data) {
       
    });

    $('#fileupload').bind('fileuploaddone', function (e, data) {
    });

});



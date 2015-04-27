/**
 * @license Copyright (c) 2003-2013, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.html or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function (config) {
    // Define changes to default configuration here.
    // For complete reference see:
    // http://docs.ckeditor.com/#!/api/CKEDITOR.config

    //the language of CKEditor 
    config.language = "en";
    config.removePlugins = 'elementspath';
    config.extraPlugins = 'colorbutton,font';
    config.contentsCss = 'Resource/ckeditor/myContent.css';
    config.allowedContent = true;


    config.toolbar = [
        ['Bold', 'Italic', 'Strike', '-', 'Undo', 'Redo', '-', 'Cut', 'Copy', 'Paste', 'Find', 'Replace', '-', 'Outdent', 'Indent', '-', 'Print'],
        ['NumberedList', 'BulletedList', '-', 'Link', 'Source'],
        ['Maximize'], '/',
        ['Styles', 'Format', 'Font', 'FontSize'],
         { name: 'colors', items: ['TextColor', 'BGColor'] }
   
    ];




};

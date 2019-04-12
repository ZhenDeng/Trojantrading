/// <reference path="../../../node_modules/@types/jquery/index.d.ts" />
/// <reference path="../../../node_modules/@types/jqueryui/index.d.ts" />
/// <reference path="./jquery.extension.d.ts" />

import { Injectable } from '@angular/core';

declare var jquery: any;
declare var $: any;

@Injectable()
export class ShareService {

  constructor() { }

  savecookies(name: string, value: string, mins: number) {
    this.createCookie(name, value, mins);
  }
  createCookie(name: string, value: string, mins: number) {
    var expires = "";
    if (mins) {
      var date = new Date();
      date.setTime(date.getTime() + (mins * 60 * 1000));
      expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + value + expires + "; path=/";
  }
  readCookie(name: string) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
      var c = ca[i];
      while (c.charAt(0) == ' ') c = c.substring(1, c.length);
      if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
  }

  showError = (
    selector: string,
    message: string,
    position: 'top' | 'right' | 'bottom' | 'left',
  ) => {
    this.showValidator(selector, message, position, 'error', true, true);
  }

  showSuccess = (
    selector: string,
    message: string,
    position: 'top' | 'right' | 'bottom' | 'left',
  ) => {
    this.showValidator(selector, message, position, 'success', true, false);
  }

  showValidator = (
    selector: string,
    message: string,
    position: 'top' | 'right' | 'bottom' | 'left',
    messageType: 'success' | 'error' | 'confirm' | 'warning',
    isStayOut: boolean = false,
    isStayAlive: boolean = false,
    finishTime?: number,
    confirmCallback?: Function,
    confirmOptions?: string[],
    confirmTheme?: string
  ) => {
    console.info($(selector));
    $(selector).showValidator(message, messageType, position, isStayOut, isStayAlive, finishTime, confirmCallback, confirmTheme, confirmOptions);
  }
}

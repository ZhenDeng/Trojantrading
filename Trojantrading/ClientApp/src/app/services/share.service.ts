/// <reference path="../../../node_modules/@types/jquery/index.d.ts" />
/// <reference path="../../../node_modules/@types/jqueryui/index.d.ts" />
/// <reference path="./jquery.extension.d.ts" />

import { Injectable } from '@angular/core';
import { Product } from '../models/Product';

declare var jquery: any;
declare var $: any;

@Injectable()
export class ShareService {

  product: Product[] = [
    {id: 1, name: "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor.", originalPrice: 1.5, vipTwoPrice: 1.5, vipOnePrice: 1.0079, quantity: 1, category: 'H', button: "Update Cart", status: 'New'},
    {id: 1, name:  "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor.", originalPrice: 1.5, vipTwoPrice: 1.5, vipOnePrice: 4.0026, quantity: 1, category: 'He', button: "Update Cart", status: 'New'},
    {id: 1, name:  "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor.", originalPrice: 1.5, vipTwoPrice: 1.5, vipOnePrice: 6.941, quantity: 1, category: 'Li', button: "Update Cart", status: 'New'},
    {id: 1, name:  "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor.", originalPrice: 1.5, vipTwoPrice: 1.5, vipOnePrice: 9.0122, quantity: 1, category: 'Be', button: "Update Cart", status: 'New'},
    {id: 1, name:  "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor.", originalPrice: 1.5, vipTwoPrice: 1.5, vipOnePrice: 10.811, quantity: 1, category: 'B', button: "Update Cart", status: 'New'},
    {id: 1, name:  "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor.", originalPrice: 1.5, vipTwoPrice: 1.5, vipOnePrice: 12.0107, quantity: 1, category: 'C', button: "Update Cart", status: 'New'},
    {id: 1, name:  "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor.", originalPrice: 1.5, vipTwoPrice: 1.5, vipOnePrice: 14.0067, quantity: 1, category: 'N', button: "Update Cart", status: 'New'},
    {id: 1, name:  "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor.", originalPrice: 1.5, vipTwoPrice: 1.5, vipOnePrice: 15.9994, quantity: 1, category: 'O', button: "Update Cart", status: 'New'},
    {id: 1, name:  "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor.", originalPrice: 1.5, vipTwoPrice: 1.5, vipOnePrice: 18.9984, quantity: 1, category: 'F', button: "Update Cart", status: 'New'},
    {id: 1, name:  "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor.", originalPrice: 1.5, vipTwoPrice: 1.5, vipOnePrice: 20.1797, quantity: 1, category: 'Ne', button: "Update Cart", status: 'New'}
  ]

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
    $(selector).showValidator(message, messageType, position, isStayOut, isStayAlive, finishTime, confirmCallback, confirmTheme, confirmOptions);
  }
}

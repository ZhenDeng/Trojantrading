import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material';

declare var jquery: any;
declare var $: any;

@Injectable()
export class ShareService {

  constructor(private _snackBar: MatSnackBar) { }

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

openSnackBar = (
      message: string
    , messageType: 'success' | 'error'
) =>  {
  this._snackBar.open(message, "Dismiss", {
      duration: 1500,
      data: {message: message, messageType: messageType},
      horizontalPosition: "center",
      verticalPosition: "bottom",
      panelClass: messageType
    });
  }
}

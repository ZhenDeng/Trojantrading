import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';

import { Observable } from 'rxjs/Observable';
import { Router } from '@angular/router';
import { ShareService } from '../services/share.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private router: Router, private shareService: ShareService) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
  
    if(request.headers.get('No-Auth') == "True"){
        return next.handle(request.clone());
    }

    if(this.shareService.readCookie("userToken")) {
        return next.handle(
            request.clone(
                { headers: request.headers.set("Authorization", "Bearer " + this.shareService.readCookie("userToken"))}
            )
        );
    }
    else{
        this.router.navigateByUrl('/login');
    }

  }
}
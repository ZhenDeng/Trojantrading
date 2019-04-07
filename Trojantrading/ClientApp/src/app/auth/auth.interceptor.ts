import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';

import { Observable } from 'rxjs/Observable';
import { Router } from '@angular/router';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private router: Router) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
  
    if(request.headers.get('No-Auth') == "True"){
        return next.handle(request.clone());
    }

    if(localStorage.getItem('userToken') != null) {
        return next.handle(
            request.clone(
                { headers: request.headers.set("Authorization", "Bearer " + localStorage.getItem('userToken'))}
            )
        );
    }
    else{
        this.router.navigateByUrl('/login');
    }

  }
}
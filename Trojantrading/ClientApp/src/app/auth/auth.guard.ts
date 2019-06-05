import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { ShareService } from '../services/share.service';

@Injectable()
export class AuthGuard implements CanActivate {
  constructor(private router: Router, private shareService: ShareService) { }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): boolean {
    if (this.shareService.readCookie("userToken")) {
      if (this.shareService.readCookie("role") != "admin" && this.shareService.readCookie("website") == "down") {
        this.router.navigate(['/maintain']);
        return false;
      } else {
        return true;
      }
    }
    else {
      if (this.shareService.readCookie("role") != "admin" && this.shareService.readCookie("website") == "down") {
        this.router.navigate(['/maintain']);
        return false;
      } else {
        this.router.navigate(['/login']);
        return false;
      }
    }
  }
}

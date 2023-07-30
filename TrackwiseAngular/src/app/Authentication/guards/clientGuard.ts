import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { DataService } from 'src/app/services/data.service';

@Injectable({
  providedIn: 'root'
})
export class ClientGuard implements CanActivate {

  constructor(private dataService: DataService, private router: Router) {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    // Check if the user is logged in and has the role of "Admin"
    const role = JSON.parse(sessionStorage.getItem("Role")!);
    if (role === "Client") {
      return true; // Allow access to the route
    } else {
      // Redirect to the login page or a page with an "Access Denied" message
      this.router.navigateByUrl('Authentication/login');
      return false;
    }
  }
}

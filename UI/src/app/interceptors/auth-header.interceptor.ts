import {Injectable} from "@angular/core";
import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from "@angular/common/http";
import {Observable} from "rxjs";
import {AccountService} from "../services/account.service";

@Injectable()
export class AuthHeaderInterceptor implements HttpInterceptor {
  constructor(private authService: AccountService) {
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    const authReq = req.clone({
      headers: req.headers.set('X-User-Name', localStorage.getItem('user') ?? '')
    });

    console.log('Intercepted HTTP call', authReq);

    return next.handle(authReq);
  }
}

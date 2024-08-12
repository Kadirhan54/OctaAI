import { Injectable } from '@angular/core';
import { HttpEvent, HttpRequest, HttpInterceptorFn, HttpHandlerFn } from '@angular/common/http';
import { Observable } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (req: HttpRequest<any>, next: HttpHandlerFn): Observable<HttpEvent<any>> => {
  const token = localStorage.getItem('token');

    console.log('intercepted request ... :TOKEN ', token);
    if (token) {
      const cloned = req.clone({
        headers: req.headers.set('Authorization', `Bearer ${token}`)
      });

      return next(cloned);
    } else {
      return next(req);
    }
};

// @Injectable()
// export class HttpInterceptorService implements HttpInterceptor {

//   intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
//     const token = localStorage.getItem('token');

//     console.log('intercepted request ... :TOKEN ', token);
//     if (token) {
//       const cloned = req.clone({
//         headers: req.headers.set('Authorization', `Bearer ${token}`)
//       });

//       return next.handle(cloned);
//     } else {
//       return next.handle(req);
//     }
//   }
// }


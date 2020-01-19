import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpHeaders, HttpResponse } from '@angular/common/http';
import { of, Observable } from 'rxjs';
import { tap, startWith } from 'rxjs/operators';

// start from https://github.com/angular/angular/blob/master/aio/content/examples/http/src/app/http-interceptors/caching-interceptor.ts

@Injectable()
export class CachingInterceptor implements HttpInterceptor {
  cache: Map<string, any> = new Map<string, any>();
  constructor() {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // continue if not cachable.
    if (this.isCachable(req)) { return next.handle(req); }
    let obsCache: HttpResponse<any> = null;
    if (this.cache.has(req.url)) {
        console.log('exists in cache');
        const cachedResponse = this.cache.get(req.url);
        obsCache =  new HttpResponse({ body: cachedResponse });
    }

    let sendDataAndCache$ = this.sendRequestAndCache(req, next);
    if(obsCache) {
       sendDataAndCache$ = sendDataAndCache$.pipe(
           tap(e => {
               console.log('from cache');
           }),
            startWith(obsCache)
           );
    }
    return sendDataAndCache$;

  }
  sendRequestAndCache(
    req: HttpRequest<any>,
    next: HttpHandler,
    ): Observable<HttpEvent<any>> {

    return next.handle(req).pipe(
      tap(event => {
        // There may be other events besides the response.
        if (event instanceof HttpResponse) {
          this.cache[req.url] = event.body; // Update the cache.
        }
      })
    );
  }
  isCachable(req: HttpRequest<any>): boolean {
    // Only GET requests are cachable
    return req.method === 'GET'; // &&
      // if you want to cache some urls
     // -1 < req.url.indexOf(searchUrl);
  }
}

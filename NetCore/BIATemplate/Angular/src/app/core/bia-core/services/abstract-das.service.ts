import { HttpClient, HttpHeaders, HttpParams, HttpResponse } from '@angular/common/http';
import { Injector } from '@angular/core';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { LazyLoadEvent } from 'primeng/api';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';

export interface HttpOptions {
  headers?:
    | HttpHeaders
    | {
        [header: string]: string | string[];
      };
  observe?: any;
  params?:
    | HttpParams
    | {
        [param: string]: string | string[];
      };
  reportProgress?: boolean;
  responseType?: any;
  withCredentials?: boolean;
}

export abstract class AbstractDas<TOut, TIn = Pick<TOut, Exclude<keyof TOut, 'id'>>> {
  protected http: HttpClient;
  protected route: string;

  constructor(injector: Injector, endpoint: string) {
    this.http = injector.get<HttpClient>(HttpClient);
    this.route = AbstractDas.buildRoute(endpoint);
  }

  public static buildRoute(endpoint: string): string {
    let route = '/' + endpoint + '/';
    route = route.replace('//', '/');
    return environment.apiUrl + route;
  }

  get(id: string | number, options?: HttpOptions): Observable<TOut> {
    return this.http.get<TOut>(`${this.route}${id}`, options).pipe(
      map((data) => {
        this.fillDate(data);
        return data;
      })
    );
  }

  getList(options?: HttpOptions): Observable<TOut[]> {
    return this.http.get<TOut[]>(`${this.route}`, options).pipe(
      map((datas) => {
        datas.forEach((data) => {
          this.fillDate(data);
        });
        return datas;
      })
    );
  }

  getListByPost(event: LazyLoadEvent, endpoint: string = 'all'): Observable<DataResult<TOut[]>> {
    if (!event) {
      return of();
    }
    return this.http.post<TOut[]>(`${this.route}${endpoint}`, event, { observe: 'response' }).pipe(
      map((resp: HttpResponse<TOut[]>) => {
        const totalCount = Number(resp.headers.get('X-Total-Count'));
        const datas = resp.body ? resp.body : [];
        datas.forEach((data) => {
          this.fillDate(data);
        });

        const dataResult = {
          totalCount,
          data: datas
        } as DataResult<TOut[]>;
        return dataResult;
      })
    );
  }

  save(items: TIn[], endpoint: string = 'save', options?: HttpOptions) {
    return this.http.post<TOut>(`${this.route}${endpoint}`, items, options);
  }

  put(item: TIn, id: string | number, options?: HttpOptions) {
    return this.http.put<TOut>(`${this.route}${id}`, item, options);
  }

  post(item: TIn, options?: HttpOptions) {
    return this.http.post<TOut>(this.route, item, options);
  }

  delete(id: string | number, options?: HttpOptions) {
    return this.http.delete<void>(`${this.route}${id}`, options);
  }

  getFile(event: LazyLoadEvent, endpoint: string = 'csv'): Observable<any> {
    return this.http.post(`${this.route}${endpoint}`, event, {
      responseType: 'blob',
      headers: new HttpHeaders().append('Content-Type', 'application/json')
    });
  }

  private isDate(value: any): boolean {
    const regex = /(\d{4})-(\d{2})-(\d{2})T/;
    if (typeof value === 'string' && value.match(regex)) {
      const date = Date.parse(value);
      return !isNaN(date);
    }
    return false;
  }

  private fillDate(data: TOut) {
    Object.keys(data).forEach((key: string) => {
      if (this.isDate((data as any)[key])) {
        (data as any)[key] = new Date((data as any)[key]);
      }
    });
  }
}

import axios, { AxiosRequestConfig } from "axios";

export enum HttpRequestMethod {
  GET = 1,
  POST,
  PUT,
  DELETE,
  PATCH,
}

export class HttpConfig implements AxiosRequestConfig {
  constructor(data?: HttpConfig) {
    Object.assign(this, data);
  }
}

export class ResponseSchema {
  public data?: any;
  public status?: any;
  public statusText?: string;
  public headers?: Array<string>;
  config?: any;
  request?: any;
}

export class HttpRequestDispatcher {
  private static httpClient: any;

  public static SendRequest<T, K>(
    method: HttpRequestMethod,
    url: string,
    data?: T,
    customHttpClient?: any
  ): Promise<K> {
    customHttpClient
      ? (this.httpClient = customHttpClient)
      : (this.httpClient = axios);

    switch (method) {
      case HttpRequestMethod.GET: {
        return this.Get(url, data, this.httpClient);
      }
      case HttpRequestMethod.POST: {
        return this.Post(url, data, this.httpClient);
      }
      case HttpRequestMethod.PUT: {
        return this.Put(url, data, this.httpClient);
      }
      case HttpRequestMethod.DELETE: {
        return this.Delete(url, data, this.httpClient);
      }
      case HttpRequestMethod.PATCH: {
        return this.Patch(url, data, this.httpClient);
      }
    }
  }

  public static Get<T, K>(url: string, data: T, httpClient: any): Promise<K> {
    const config = new HttpConfig({
      url: url,
      method: "get",
      params: data,
      timeout: 300000
    });
    return httpClient.get(url, config);
  }

  public static Post<T, K>(url: string, data: T, httpClient: any): Promise<K> {
    const config = new HttpConfig({
      maxContentLength: 100000000,
      maxBodyLength: 1000000000,
      timeout: 300000,
      headers: {
        'Content-Type': "application/json;charset=utf-8"
      }
    });
    return httpClient.post(url, data, config);
  }

  public static Put<T, K>(url: string, data: T, httpClient: any): Promise<K> {
    const config = new HttpConfig({
      maxContentLength: 100000000,
      maxBodyLength: 1000000000,
      timeout: 300000,
      headers: {
        'Content-Type': "application/json;charset=utf-8"
      }
    });
    return httpClient.put(url, data, config);
  }

  public static Delete<T, K>(
    url: string,
    data: T,
    httpClient: any
  ): Promise<K> {
    return httpClient.delete(url, data);
  }

  public static Patch<T, K>(url: string, data: T, httpClient: any): Promise<K> {
    const config = new HttpConfig({
      maxContentLength: 100000000,
      maxBodyLength: 1000000000,
      timeout: 300000,
      headers: {
        'Content-Type': "application/json;charset=utf-8"
      }
    });
    return httpClient.patch(url, data, config);
  }
}

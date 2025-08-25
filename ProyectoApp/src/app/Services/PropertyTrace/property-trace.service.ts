import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment'; 
import { ResultModel } from '../../Models/ResultModel';
import { PropertyTraceModel } from '../../Models/PropertyTraceModel';

@Injectable({
  providedIn: 'root'
})
export class PropertyTraceService {

  constructor(private http: HttpClient) { }

  public GetPropertyTraceByPropertyTraceId(id: number) {
    return this.http.post(environment.BaseUrl + "api/PropertyTrace/GetPropertyTraceByPropertyTraceId", id);
  }

  public GetAllPropertyTraces(): Observable<ResultModel> {
    return this.http.get<ResultModel>(environment.BaseUrl + "api/PropertyTrace/PropertyTraceList");
  }

  public GetAllPropertyTracesByIdProperty(id: number): Observable<ResultModel> {
    return this.http.get<ResultModel>(environment.BaseUrl + "api/PropertyTrace/PropertyTraceListByPropertyId/" + id);
  }

  public SavePropertyTrace(PropertyTrace: any) {
    return this.http.post(environment.BaseUrl + "api/PropertyTrace/PropertyTraceAdd", PropertyTrace);
  }

  public UpdatePropertyTrace(PropertyTrace: PropertyTraceModel) {
    return this.http.put(environment.BaseUrl + "api/PropertyTrace/PropertyTraceUpdt", PropertyTrace);
  }


  public DeletePropertyTrace(id: number) {
    return this.http.delete(environment.BaseUrl + "api/PropertyTrace/PropertyTraceDelete/" + id);
  }

}

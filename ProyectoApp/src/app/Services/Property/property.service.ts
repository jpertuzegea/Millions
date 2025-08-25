import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PropertyModel } from '../../Models/PropertyModel';
import { ResultModel } from '../../Models/ResultModel';
import { SearchModel } from '../../Models/SearchModel';

@Injectable({
  providedIn: 'root'
})
export class PropertyService {

  constructor(private http: HttpClient) { }

  public GetPropertyByPropertyId(id: number) {
    return this.http.post(environment.BaseUrl + "api/Property/GetPropertyByPropertyId", id);
  }

  public GetAllPropertys(): Observable<ResultModel> {
    return this.http.get<ResultModel>(environment.BaseUrl + "api/Property/PropertyList");
  }

  public SaveProperty(Property: any) {
    return this.http.post(environment.BaseUrl + "api/Property/PropertyAdd", Property);
  }

  public UpdateProperty(Property: PropertyModel) {
    return this.http.put(environment.BaseUrl + "api/Property/PropertyUpdt", Property);
  }


  public DeleteProperty(id: number) {
    return this.http.delete(environment.BaseUrl + "api/Property/PropertyDelete/" + id);
  }


  public SearchPropertys(search: SearchModel) {
    return this.http.post<ResultModel>(environment.BaseUrl + "api/Property/SearchPropertys", search);
  }

}

import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ResultModel } from '../../Models/ResultModel';
import { OwnerModel } from '../../Models/OwnerModel';

@Injectable({
  providedIn: 'root'
})
export class OwnerService {

  constructor(private http: HttpClient) { }

  public GetOwnerByOwnerId(id: number) {
    return this.http.post(environment.BaseUrl + "api/Owner/GetOwnerByOwnerId", id);
  }

  public GetAllOwners(): Observable<ResultModel> {
    return this.http.get<ResultModel>(environment.BaseUrl + "api/Owner/OwnerList");
  }

  public SaveOwner(Owner: any) {
    console.log(Owner);
    return this.http.post(environment.BaseUrl + "api/Owner/OwnerAdd", Owner);
  }

  public UpdateOwner(Owner: OwnerModel) {
    return this.http.put(environment.BaseUrl + "api/Owner/OwnerUpdt", Owner);
  }


  public DeleteOwner(id: number) {
    return this.http.delete(environment.BaseUrl + "api/Owner/OwnerDelete/" + id);
  }
}

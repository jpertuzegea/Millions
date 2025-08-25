import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PropertyImageModel } from '../../Models/PropertyImageModel';
import { ResultModel } from '../../Models/ResultModel';

@Injectable({
  providedIn: 'root'
})

export class PropertyImageService {

  constructor(private http: HttpClient) { }

  public GetPropertyImageByPropertyImageId(id: number) {
    return this.http.post(environment.BaseUrl + "api/PropertyImage/GetPropertyImageByPropertyImageId", id);
  }

  public GetAllPropertyImages(): Observable<ResultModel> {
    return this.http.get<ResultModel>(environment.BaseUrl + "api/PropertyImage/PropertyImageList");
  }

  public GetAllPropertyImagesByIdProperty(id: number): Observable<ResultModel> {
    return this.http.get<ResultModel>(environment.BaseUrl + "api/PropertyImage/GetAllPropertyImagesByIdProperty/" + id);
  }

  public SavePropertyImage(PropertyImage: any) {
    return this.http.post(environment.BaseUrl + "api/PropertyImage/PropertyImageAdd", PropertyImage);
  }

  public UpdatePropertyImage(PropertyImage: PropertyImageModel) {
    return this.http.put(environment.BaseUrl + "api/PropertyImage/PropertyImageUpdt", PropertyImage);
  }


  public DeletePropertyImage(id: number) {
    return this.http.delete(environment.BaseUrl + "api/PropertyImage/PropertyImageDelete/" + id);
  }

}

import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UsersAdminService {
  private apiUrl = `${environment.apiUrl}/users`;

  constructor(private http: HttpClient) { }

  getAllUsers(): Observable<UserDto[]> {
    return this.http.get<UserDto[]>(this.apiUrl);
  }

  createUser(userCreateParams: UserCreateParams): Observable<UserDto> {
    return this.http.post<UserDto>(this.apiUrl, userCreateParams);
  }

  updateUser(id: string, userCreateParams: UserCreateParams): Observable<UserDto> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.put<UserDto>(url, userCreateParams);
  }

  deleteUser(id: string): Observable<{}> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.delete(url);
  }
}
export interface UserCreateParams {
  name: string;
  email: string;
  credits: number;
}

export interface UserDto {
  id: string;
  name: string;
  email: string;
  credits: number;
}

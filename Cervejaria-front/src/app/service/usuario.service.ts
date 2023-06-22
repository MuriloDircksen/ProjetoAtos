import { Injectable } from '@angular/core';
import { Observable, catchError, retry, throwError } from 'rxjs';
import { IUsuario } from '../models/usuario';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class UsuarioService {

  //private url:string = "http://localhost:3000/usuarios";
  private url:string = "https://localhost:7227/api/usuarios";

  constructor(private http: HttpClient) { }
  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  }

  getUsuarios():Observable<IUsuario[]>{
    return this.http.get<IUsuario[]>(this.url)
    .pipe(
      retry(2),
      catchError(this.handleError));
  }

  salvarUsuario(usuario: IUsuario): Observable<IUsuario>{
    return this.http.post<IUsuario>(this.url, JSON.stringify(usuario), this.httpOptions)
    .pipe(
      retry(2),
      catchError(this.handleError));
  }

  handleError(error: HttpErrorResponse) {
    let errorMessage = '';
    if (error.error instanceof ErrorEvent) {
      // Erro ocorreu no lado do client
      errorMessage = error.error.message;
    } else {
      // Erro ocorreu no lado do servidor
      errorMessage = `Código do erro: ${error.status}, ` +
      `mensagem: ${error.message}`;
    }
    console.log(errorMessage);
    //window.alert(errorMessage);
    return throwError(errorMessage);
  };
}

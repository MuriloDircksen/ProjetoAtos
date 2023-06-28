import { Router } from '@angular/router';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, retry, throwError } from 'rxjs';
import { IEstoque } from 'src/app/models/estoque';
import { TokenService } from '../token/token.service';


@Injectable({
  providedIn: 'root'
})
export class EstoqueServiceService {

  //private url:string = "http://localhost:3000/estoques";
  private url:string = "https://localhost:7227/api/estoques";

  constructor(private http: HttpClient, private tokenService : TokenService, private router : Router) { }

    token = this.tokenService.getToken();
    httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json', 'Authorization': `Bearer ${this.token}` })
    }

  getEstoques():Observable<IEstoque[]>{

    return this.http.get<IEstoque[]>(this.url, this.httpOptions)
    .pipe(
      retry(2),
      catchError(this.handleError));
  }
  getEstoque(id : number):Observable<IEstoque>{
    return this.http.get<IEstoque>(`${this.url}/${id}`, this.httpOptions)
    .pipe(
      retry(2),
      catchError(this.handleError));
  }
  salvarEstoque(estoque: IEstoque): Observable<IEstoque>{
    return this.http.post<IEstoque>(this.url, JSON.stringify(estoque), this.httpOptions)
    .pipe(
      retry(2),
      catchError(this.handleError));
  }
  atualizarEstoque(estoque: IEstoque): Observable<IEstoque> {
    return this.http.put<IEstoque>(`${this.url}/${estoque.id}`, JSON.stringify(estoque), this.httpOptions)
    .pipe(
      retry(2),
      catchError(this.handleError));
  }

  excluirEstoque(id: number): Observable<IEstoque> {
    return this.http.delete<any>(`${this.url}/${id}`, this.httpOptions)
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
    if(error.status ==401){
      window.alert("Token venceu!");
      this.router.navigate(['/login']);
    }
    console.log(errorMessage);
    //window.alert(errorMessage);
    return throwError(errorMessage);
  };
}

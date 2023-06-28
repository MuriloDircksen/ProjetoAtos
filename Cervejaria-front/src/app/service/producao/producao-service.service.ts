import { Router } from '@angular/router';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, retry, throwError } from 'rxjs';
import { IProducao } from 'src/app/models/producao';
import { TokenService } from '../token/token.service';

@Injectable({
  providedIn: 'root'
})
export class ProducaoServiceService {

  //private url:string = "http://localhost:3000/producao";
  private url:string = "https://localhost:7227/api/producoes";

  constructor(private http: HttpClient, private tokenService : TokenService, private router : Router) { }

 token = this.tokenService.getToken();
 httpOptions = {
   headers: new HttpHeaders({ 'Content-Type': 'application/json', 'Authorization': `Bearer ${this.token}` })
 }

  getProducoes():Observable<IProducao[]>{
    return this.http.get<IProducao[]>(this.url, this. httpOptions)
    .pipe(
      retry(2),
      catchError(this.handleError));
  }
  getProducao(id:number):Observable<IProducao[]>{
    return this.http.get<IProducao[]>(`${this.url}/${id}`, this. httpOptions)
    .pipe(
      retry(2),
      catchError(this.handleError));
  }

  salvarProducao(producao: IProducao): Observable<IProducao>{
    return this.http.post<IProducao>(this.url, JSON.stringify(producao), this. httpOptions)
    .pipe(
      retry(2),
      catchError(this.handleError));
  }
  atualizarProducao(producao: IProducao): Observable<IProducao> {
    return this.http.put<IProducao>(`${this.url}/${producao.id}`, JSON.stringify(producao), this. httpOptions)
    .pipe(
      retry(2),
      catchError(this.handleError));
  }
  excluirProducao(id: number): Observable<IProducao> {
    return this.http.delete<any>(`${this.url}/${id}`, this. httpOptions)
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
   // window.alert(errorMessage);
    return throwError(errorMessage);
  };
}

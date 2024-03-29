import { Router } from '@angular/router';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, retry, throwError } from 'rxjs';
import { IIngredientes } from 'src/app/models/ingredientes';
import { TokenService } from '../token/token.service';


@Injectable({
  providedIn: 'root'
})
export class IngredienteServiceService {

  //private url:string = "http://localhost:3000/ingredientes";
  private url:string = "https://localhost:7227/api/ingredientes";

  constructor(private http: HttpClient, private tokenService : TokenService, private router : Router) { }

 token = this.tokenService.getToken();
 httpOptions = {
   headers: new HttpHeaders({ 'Content-Type': 'application/json', 'Authorization': `Bearer ${this.token}` })
 }

  getIngredientes():Observable<IIngredientes[]>{
    return this.http.get<IIngredientes[]>(this.url, this.httpOptions)
    .pipe(
      retry(2),
      catchError(this.handleError));
  }

  getIngrediente(id:number):Observable<IIngredientes[]>{
    return this.http.get<IIngredientes[]>(`${this.url}/${id}`, this.httpOptions)
    .pipe(
      retry(2),
      catchError(this.handleError));
  }
  salvarIngrediente(ingrediente: IIngredientes): Observable<IIngredientes>{
    return this.http.post<IIngredientes>(this.url, JSON.stringify(ingrediente), this.httpOptions)
    .pipe(
      retry(2),
      catchError(this.handleError));
  }
  atualizarIngrediente(ingrediente: IIngredientes): Observable<IIngredientes> {
    return this.http.put<IIngredientes>(`${this.url}/${ingrediente.id}`, JSON.stringify(ingrediente), this.httpOptions)
    .pipe(
      retry(2),
      catchError(this.handleError));
  }

  excluirIngrediente(id: number): Promise<IIngredientes> {
    return this.http.delete<any>(`${this.url}/${id}`, this.httpOptions)
    .pipe(
      retry(2),
      catchError(this.handleError)).toPromise();
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

import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, retry, throwError } from 'rxjs';
import { IIngredientes } from 'src/app/models/ingredientes';


@Injectable({
  providedIn: 'root'
})
export class IngredienteServiceService {

  //private url:string = "http://localhost:3000/ingredientes";
  private url:string = "https://localhost:7227/api/ingredientes";

  constructor(private http: HttpClient) { }
  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  }

  getIngredientes():Observable<IIngredientes[]>{
    return this.http.get<IIngredientes[]>(this.url)
    .pipe(
      retry(2),
      catchError(this.handleError));
  }

  getIngrediente(id:number):Observable<IIngredientes[]>{
    return this.http.get<IIngredientes[]>(`${this.url}/${id}`)
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

  excluirIngrediente(id: number): Observable<IIngredientes> {
    return this.http.delete<any>(`${this.url}/${id}`)
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

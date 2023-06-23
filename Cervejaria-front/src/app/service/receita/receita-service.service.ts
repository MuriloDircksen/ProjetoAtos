import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, retry,  throwError } from 'rxjs';
import { IReceitaIngrediente } from 'src/app/models/receita-ingrediente';
import { IReceitas } from 'src/app/models/receitas';

@Injectable({
  providedIn: 'root'
})
export class ReceitasServiceService {

 // private url:string = "http://localhost:3000/receitas";
 // private url2:string ="http://localhost:3000/receita_ingrediente"
 private url :string = "https://localhost:7227/api/receitas";
 private url2:string = "https://localhost:7227/api/receitaingredientes"

  constructor(private http: HttpClient) { }
  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  }

  getReceitas():Observable<IReceitas[]>{
    return this.http.get<IReceitas[]>(this.url)
    .pipe(
      retry(2),
      catchError(this.handleError));
  }

  getReceitaIngredientes():Observable<IReceitaIngrediente[]>{
    return this.http.get<IReceitaIngrediente[]>(this.url2)
    .pipe(
      retry(2),
      catchError(this.handleError));
  }
  getReceita(id: number): Observable<IReceitas> {
    return this.http.get<IReceitas>(`${this.url}/${id}`)
    .pipe(
      retry(2),
      catchError(this.handleError))
  }
  salvarReceita(receita: IReceitas): Observable<IReceitas>{
    return this.http.post<IReceitas>(this.url, JSON.stringify(receita), this.httpOptions)
    .pipe(
      retry(2),
      catchError(this.handleError));
  }
  salvarReceitaIngredientes(receitaIngrediente : IReceitaIngrediente) :Observable<IReceitaIngrediente>{
    return this.http.post<IReceitaIngrediente>(this.url2, JSON.stringify(receitaIngrediente), this.httpOptions)
    .pipe(
      retry(2),
      catchError(this.handleError));
  }

  atualizarReceita(receita: IReceitas): Observable<IReceitas> {
    return this.http.put<IReceitas>(`${this.url}/${receita.id}`, JSON.stringify(receita), this.httpOptions)
    .pipe(
      retry(1),
      catchError(this.handleError));
  }
  atualizarReceitaIngrediente(receitaIngrediente : IReceitaIngrediente) :Observable<IReceitaIngrediente>{
    return this.http.put<IReceitaIngrediente>(`${this.url2}/${receitaIngrediente.id}`, JSON.stringify(receitaIngrediente), this.httpOptions)
    .pipe(
      retry(1),
      catchError(this.handleError));
  }
  excluirReceita(id: number): Observable<IReceitas> {
    return this.http.delete<any>(`${this.url}/${id}`)
    .pipe(
      retry(1),
      catchError(this.handleError));
  }
  excluirReceitaIngrediente(id: number): Observable<IReceitas> {
    return this.http.delete<any>(`${this.url2}/${id}`)
    .pipe(
      retry(1),
      catchError(this.handleError));
  }
  excluirEspecialReceitaIngrediente(id: number): Observable<IReceitas> {
    return this.http.delete<any>(`${this.url2}/atualizar/${id}`)
    .pipe(
      retry(1),
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

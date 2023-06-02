import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IIngredientes } from 'src/app/models/ingredientes';


@Injectable({
  providedIn: 'root'
})
export class IngredienteServiceService {

  private url:string = "http://localhost:3000/ingredientes";

  constructor(private http: HttpClient) { }

  getIngredientes():Observable<IIngredientes[]>{
    return this.http.get<IIngredientes[]>(this.url);
  }

  salvarIngrediente(ingrediente: IIngredientes): Observable<IIngredientes>{
    return this.http.post<IIngredientes>(this.url, ingrediente);
  }
  atualizarIngrediente(ingrediente: IIngredientes): Observable<IIngredientes> {
    return this.http.put<IIngredientes>(`${this.url}/${ingrediente.id}`, ingrediente);
  }

  excluirIngrediente(id: number): Observable<IIngredientes> {
    return this.http.delete<any>(`${this.url}/${id}`);
  }
}

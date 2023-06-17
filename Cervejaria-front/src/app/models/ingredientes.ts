export interface IIngredientes{
  id?: number,
  nomeIngrediente: string,
  idEstoque: number,
  quantidade: number,
  tipo: string,
  valor_unidade: number,
  unidade: string,
  fornecedor: string,
  validade: Date,
  data_entrada: Date
}

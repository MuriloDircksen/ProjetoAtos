export interface IIngredientes{
  id?: number,
  nomeIngrediente: string,
  idEstoque: number,
  quantidade: number,
  tipo: string,
  valorUnidade: number,
  valorTotal: number,
  unidade: string,
  fornecedor: string,
  validade: Date,
  dataEntrada: Date
}

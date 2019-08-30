import axios from '@/libs/api.request'
import store from '@/store'

var token=store.state.user.token;

//分页
export const getPage = ({pageNum ,  pageSize ,  field ,  order,query={}  }) => {
  const data = {
    pageNum:pageNum,
    pageSize:pageSize,
    field:field,
    order:order,
    query:query
  }
  return axios.request({
    url: 'api/PubUser/GetPage',
   // headers: {Authorization:"Bearer "+token},
    data,
    method: 'post'
  })
 
}

//添加
export const add=(model)=>{
  const data=model;
  return axios.request({
    url: 'api/PubUser/Add',
    headers: {Authorization:"Bearer "+token},
    data,
    method: 'post'
  })
}

//修改
export const edit=(model)=>{
  const data=model;
  return axios.request({
    url: 'api/PubUser/Edit',
    headers: {Authorization:"Bearer "+token},
    data,
    method: 'post'
  })
}

//停用
export const remove=(id)=>{
  return axios.request({
    url: 'api/PubUser/Delete/'+id,
    headers: {Authorization:"Bearer "+token},
    method: 'post'
  })
}

//用户权限列表
export const getFunctions=(code)=>{
  return axios.request({
    url: 'api/PubUser/GetFunctions/'+code,
    headers: {Authorization:"Bearer "+token},
    method: 'post'
  })
}

//保存权限
export const saveFunctions=(code,data)=>{
  return axios.request({
    url: 'api/PubUser/saveFunctions/'+code,
    data,
    headers: {Authorization:"Bearer "+token},
    method: 'post'
  })
}
import axios from '@/libs/api.request'
import store from '@/store'

var token=store.state.user.token;

//获取所有角色
export const getRoles = () => {
  return axios.request({
    url: 'api/PubRole/GetList',
    headers: {Authorization:"Bearer "+token},
    method: 'post'
  })
}

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
    url: 'api/PubRole/GetPage',
    headers: {Authorization:"Bearer "+token},
    data,
    method: 'post'
  })
 
}

//添加
export const add=(model)=>{
  const data=model;
  return axios.request({
    url: 'api/PubRole/Add',
    headers: {Authorization:"Bearer "+token},
    data,
    method: 'post'
  })
}

//修改
export const edit=(model)=>{
  const data=model;
  return axios.request({
    url: 'api/PubRole/Edit',
    headers: {Authorization:"Bearer "+token},
    data,
    method: 'post'
  })
}

//停用
export const remove=(id)=>{
  return axios.request({
    url: 'api/PubRole/Delete/'+id,
    headers: {Authorization:"Bearer "+token},
    method: 'post'
  })
}
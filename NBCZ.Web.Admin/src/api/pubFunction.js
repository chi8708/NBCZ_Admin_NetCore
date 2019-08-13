import axios from '@/libs/api.request'
import store from '@/store'

var token=store.state.user.token;
export const getFunctions = () => {
  return axios.request({
    url: 'api/PubFunction/GetList',
    headers: {Authorization:"Bearer "+token},
    method: 'post'
  })
}

//获取子权限
export const getChildList=(id)=>{
  return axios.request({
    url: 'api/PubFunction/GetChildList/'+id,
    headers: {Authorization:"Bearer "+token},
    method: 'post'
  })
}

//添加
export const add=(model)=>{
  const data=model;
  return axios.request({
    url: 'api/PubFunction/Add',
    headers: {Authorization:"Bearer "+token},
    data,
    method: 'post'
  })
}

//修改
export const edit=(model)=>{
  const data=model;
  return axios.request({
    url: 'api/PubFunction/Edit',
    headers: {Authorization:"Bearer "+token},
    data,
    method: 'post'
  })
}

//停用
export const remove=(id)=>{
  return axios.request({
    url: 'api/PubFunction/Delete/'+id,
    headers: {Authorization:"Bearer "+token},
    method: 'post'
  })
}
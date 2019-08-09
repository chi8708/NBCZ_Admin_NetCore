import axios from '@/libs/api.request'
import store from '@/store'

var token=store.state.user.token;
export const getDepts = () => {
  return axios.request({
    url: 'api/PubDept/GetList',
    headers: {Authorization:"Bearer "+token},
    method: 'post'
  })
}

//获取子部门
export const getChildList=(id)=>{
  return axios.request({
    url: 'api/PubDept/GetChildList/'+id,
    headers: {Authorization:"Bearer "+token},
    method: 'post'
  })
}

//添加
export const add=(model)=>{
  const data=model;
  return axios.request({
    url: 'api/PubDept/Add',
    headers: {Authorization:"Bearer "+token},
    data,
    method: 'post'
  })
}

//修改
export const edit=(model)=>{
  const data=model;
  return axios.request({
    url: 'api/PubDept/Edit',
    headers: {Authorization:"Bearer "+token},
    data,
    method: 'post'
  })
}

//停用
export const remove=(id)=>{
  return axios.request({
    url: 'api/PubDept/Delete/'+id,
    headers: {Authorization:"Bearer "+token},
    method: 'post'
  })
}
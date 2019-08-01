import axios from '@/libs/api.request'
import store from '@/store'

var token=store.state.user.token;

//分页
export const getPage = ({pageNum ,  pageSize ,  field ,  order  }) => {
  const data = {
    pageNum:pageNum,
    pageSize:pageSize,
    field:field,
    order:order
  }
  return axios.request({
    url: 'api/PubUser/GetPage',
    headers: {Authorization:"Bearer "+token},
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
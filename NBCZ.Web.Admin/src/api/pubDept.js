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
<template>
  <div>
    <!-- Id, UserCode, UserName, RealName, UserPwd, Sex, IdentityNo, Birthday, DeptCode, ManagerFlag, Tel, EMail, QQ, Remark, StopFlag, Crid, Crdt, Lmid, Lmdt, LoginDate, ProvinceCode, CityCode, RegionCode, UserAddress, Wxcode, HeadUrl -->
    <Form label-position="right" :model="Row" :rules="ruleUser" :label-width="100">
      <Row>
        <Col span="12">
          <FormItem label="用户名" prop="userName">
            <Input v-model="Row.userName" />
          </FormItem>
        </Col>
        <Col span="12">
          <FormItem label="密码" prop="userPwd">
            <Input type="password" v-model="Row.userPwd" />
          </FormItem>
        </Col>
      </Row>
      <Row>
        <Col span="12">
          <FormItem label="电话">
            <Input v-model="Row.tel" />
          </FormItem>
        </Col>
        <Col span="12">
          <FormItem label="部门">
             <Input search enter-button v-model="Row.deptName" readonly @on-search="DeptSelect" />
            <!-- <Tree :data="DeptTree"></Tree> -->
            <!-- <Input type="password" v-model="Row.deptCode" /> -->
          </FormItem>
        </Col>
      </Row>
      <Row>
        <Col span="12">
          <FormItem label="姓名">
            <Input v-model="Row.realName" />
          </FormItem>
        </Col>
        <Col span="12">
          <FormItem label="性别">
            <RadioGroup v-model="Row.sex">
              <Radio :label="1">
                <span>男</span>
              </Radio>
              <Radio :label="0">
                <span>女</span>
              </Radio>
            </RadioGroup>
          </FormItem>
        </Col>
      </Row>
      <Row>
        <Col span="24">
          <FormItem label="角色">
            <Select v-model="Row.roleCodes" filterable multiple>
              <Option
                v-for="item in Roles"
                :value="item.roleCode"
                :key="item.roleCode"
              >{{ item.roleName }}</Option>
            </Select>
          </FormItem>
        </Col>
      </Row>
      <Row>
        <Col span="24">
          <FormItem label="地址">
            <Input v-model="Row.userAddress" />
          </FormItem>
        </Col>
      </Row>
      <Row>
        <Col span="24">
          <FormItem label="备注">
            <Input v-model="Row.remark" />
          </FormItem>
        </Col>
      </Row>
      <Row  >
        <Col span="24" >
          <div style="text-align:center;">
               <Button @click="parent.modelEdit=false">取消</Button>
               <Button style="margin-left:20px;" type="primary">保存</Button>
          </div>
        
        </Col>
      </Row>
    </Form>
     
    <Modal title="选择部门"  :mask-closable="false"   v-model="modelDept" width="300"  scrollable  footer-hide>
        <Tree :data="DeptTree" @on-select-change="DeptTreeChange"></Tree>
    </Modal>
  </div>
</template>
<script>
import { getRoles } from "@/api/pubRole";
import { getDepts } from "@/api/pubDept";
export default {
  props: { editRow: Object, parent: Object },
  computed: {},
  data() {
    return {
      Row: {},
      Roles: [],
      Depts: [],
      DeptTree: [],
      DeptTreeItems: [],
      modelDept:false,
      ruleUser: {
        userName: [
          {
            required: true,
            message: "用户名必填",
            trigger: "blur"
          }
        ],
        userPwd: [
          {
            required: true,
            message: "密码必填",
            trigger: "blur"
          },
          {
            type: "string",
            min: 6,
            message: "密码长度≥6",
            trigger: "blur"
          }
        ]
      }
    };
  },
  methods: {
    SaveEdit() {
      //console.log(this.Row);
      //this.parent.modalEdit=true;
    },
    GetRoles() {
      getRoles()
        .then(res => {
          const resData = res.data;
          const data = resData.data;
          this.Roles = data;
        })
        .catch(err => {});
    },
    GetDepts() {
      getDepts()
        .then(res => {
          const resData = res.data;
          const data = resData.data;
          this.Depts = data;
          this.LoadDeptTree();
        })
        .catch(err => {});
    },
    LoadDeptTree() {
      var treeRoot = this.Depts.filter(p => p.parentCode == "");
      this.LoadDeptTreeChild(treeRoot);
    },
    LoadDeptTreeChild(treeData) {
      for (let index = 0; index < treeData.length; index++) {
        const element = treeData[index];
        this.LoadDeptTreeItem(element);
        const child = this.Depts.filter(p => p.parentCode == element.deptCode);
        if (child && child.length > 0) {
          this.LoadDeptTreeChild(child);
        }
      }
    },
    LoadDeptTreeItem(treeItemData) {

      var treeItem=
      {
        title: treeItemData.deptName,
        expand: true, 
        value: treeItemData.deptCode
      };
      this.DeptTreeItems.push(treeItem);
      if (treeItemData.parentCode) {
        //foreach 无法终止循环
          this.DeptTreeItems.every(element => {
          if (element.value == treeItemData.parentCode) {
            if (!element.children) {
              element.children = [];
            }
            element.children.push(
              treeItem
              );
            return false;
          }
          return true;
        });
      } else {
        this.DeptTree.push(
          treeItem
        );
      }
    },
    DeptSelect(){
      if(!this.Depts||this.Depts.length<=0){
        this.GetDepts();
      }

      this.modelDept=true;
    },
    DeptTreeChange(data){
      var item0=data[0];
      this.Row.deptName=item0.title;
      this.Row.deptCode=item0.value;
      this.modelDept=false;
    }
  },
  watch: {
    editRow(newVal, oldVal) {
      this.Row = Object.assign({}, newVal);
      this.Row.sex = this.Row.sex === false ? 0 : 1;
    }
  },
  mounted() {
    this.GetRoles();
  }
};
</script>


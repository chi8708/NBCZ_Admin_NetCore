<template>
  <div>
    <!-- Id, UserCode, UserName, RealName, UserPwd, Sex, IdentityNo, Birthday, DeptCode, ManagerFlag, Tel, EMail, QQ, Remark, StopFlag, Crid, Crdt, Lmid, Lmdt, LoginDate, ProvinceCode, CityCode, RegionCode, UserAddress, Wxcode, HeadUrl -->
    <Form ref="formInline" label-position="right" :model="Row" :rules="rule" :label-width="100">
      <Row>
        <Col span="24">
          <FormItem label="父级" prop="parentName" id="item-parentName">
            <Input search enter-button v-model="Row.parentName" readonly @on-search="deptSelect" />
            <!-- <Tree :data="DeptTree"></Tree> -->
            <!-- <Input type="password" v-model="Row.deptCode" /> -->
          </FormItem>
        </Col>
      </Row>
      <Row>
        <Col span="24">
          <FormItem label="名称" prop="deptName" >
            <Input v-model="Row.deptName" />
          </FormItem>
        </Col>
      </Row>
      <Row>
        <Col span="24">
          <FormItem label="备注" prop="Remark">
            <Input v-model="Row.remark" type="textarea" :autosize="{minRows: 2,maxRows: 2}" />
          </FormItem>
        </Col>
      </Row>
      <Row>
        <Col span="24">
          <div style="text-align:center;">
            <Button @click="parent.modelEdit=false">取消</Button>
            <Button style="margin-left:20px;" type="primary" @click="save('formInline')">保存</Button>
          </div>
        </Col>
      </Row>
    </Form>

    <Modal
      title="选择部门"
      :mask-closable="false"
      v-model="modelDept"
      width="300"
      scrollable
      footer-hide
    >
      <dept-tree :parent="this"></dept-tree>
    </Modal>
  </div>
</template>
<script>
import { getRoles } from "@/api/pubRole";
import { getDepts, add, edit } from "@/api/pubDept";
import DeptTree from "./DeptTree";
export default {
  props: { editRow: Object, parent: Object },
  computed: {},
  components: {
    DeptTree
  },
  data() {
    return {
      Row: {},
      Roles: [],
      Depts: [],
      DeptTree: [],
      DeptTreeItems: [],
      modelDept: false,
      rule: {
        parentName: [
          {
            required: true,
            message: "父级必选",
            trigger: "blur"
          }
        ],
        deptName: [
          {
            required: true,
            message: "名称必填",
            trigger: "blur"
          }
        ]
      }
    };
  },
  methods: {
    deptSelect() {
      this.modelDept = true;
    },
    deptChange(code, title) {
      this.Row.parentCode = code;
      this.Row.parentName = title;
      var itemParent= document.getElementById("item-parentName")
      itemParent.classList.remove("ivu-form-item-error");
     itemParent.children[1].children[1].style.display="none";
      this.modelDept = false;
    },
    save() {
      if (this.parent.isAdd) {
        this.saveAdd();
      } else {
        this.saveEdit();
      }
    },
    saveAdd() {
      this.saveValidate().then(r => {
        if (r) {
          add(this.Row)
            .then(res => {
              const resData = res.data;
              const data = resData.data;
              const code = resData.code;
              const msg = resData.msg;
              if (code == 1) {
                this.$Message.info("添加成功");
                this.parent.modelEdit = false;
                this.parent.reloadAll(this.Row.parentCode);
              } else {
                this.$Message.error({
                  content: msg,
                  duration: 10,
                  closable: true
                });
              }
            })
            .catch(err => {});
        }
      });
    },
    saveEdit() {
      this.saveValidate().then(r => {
        if (r) {
          edit(this.Row)
            .then(res => {
              const resData = res.data;
              const data = resData.data;
              const code = resData.code;
              const msg = resData.msg;
              if (code == 1) {
                this.$Message.info("编辑成功");
                this.parent.modelEdit = false;
                this.parent.reloadAll(this.Row.parentCode);
              } else {
                this.$Message.error({
                  content: msg,
                  duration: 10,
                  closable: true
                });
              }
            })
            .catch(err => {});
        }
      });
    },
    saveValidate(name = "formInline") {
      return this.$refs[name].validate(valid => {
        if (!valid) {
          this.$Message.warning("请检查表单数据！");
          return false;
        } else {
          return true;
        }
      });
    },
    handleReset(name = "formInline") {
      this.$refs[name].resetFields();
    }
  },
  watch: {
    editRow(newVal, oldVal) {
      this.Row = Object.assign({}, newVal);
    }
  },
  mounted() {}
};
</script>

